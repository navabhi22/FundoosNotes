using DatabaseLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.DBContext;
using RepositoryLayer.Entities;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        FundooContext fundooContext;
        public IConfiguration Configuration { get; }
        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.Configuration = configuration;
        }
        

        public void AddUser(UserPostModel user)
        {
            try
            {
                User userdata = new User();
                userdata.FirstName = user.FirstName;
                userdata.LastName = user.LastName;
                userdata.Email = user.Email;
                userdata.Password = EncryptPassword(user.Password);
                userdata.DateCreated= DateTime.Now;
                userdata.Address = user.Address;
                fundooContext.users.Add(userdata);
                fundooContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string LoginUser(string Email, string Password)
        {
            try
            {

                var result = fundooContext.users.Where(u => u.Email == Email).FirstOrDefault();
                string pass = StringCipher.DecodeFrom64(result.Password);

                if (pass != Password)
                {
                    return null;
                }
                return GetJWTToken(Email, result.UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private static string GetJWTToken(string email, int userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
          new Claim("email", email),
          new Claim("Userid",userID.ToString())
              }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                     new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        public bool ForgetPassword(string Email)
        {
            try
            {
                var userdata = fundooContext.users.FirstOrDefault(u => u.Email == Email);
                if (userdata == null)
                {
                    return false;
                }

                MessageQueue queue;
                //Add message to queue
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                {
                    queue = new MessageQueue(@".\Private$\FundooQueue");
                }

                else
                {
                    queue = MessageQueue.Create(@".\Private$\FundooQueue");
                }

                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = GetJWTToken(Email, userdata.UserId);
                message.Label = "Forgot password Email";
                queue.Send(message);

                Message msg = queue.Receive();
                msg.Formatter = new BinaryMessageFormatter();
                EmailService.SendMail(Email, message.Body.ToString());
                queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);

                queue.BeginReceive();
                queue.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                      "Queue might be a system queue.");
                }
            }
        }
        public string GenerateToken(string Email)
        {
            if (Email == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
          new Claim("Email", Email)
              }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
              new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // Method to change password
        public static string EncryptPassword(string Password)
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                {
                    return null;

                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(Password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool ChangePassword(string Email, ChangePasswordModel changePassword)
        {
            try
            {
                if (changePassword.Password.Equals(changePassword.ConfirmPassword))
                {
                    var user = fundooContext.users.Where(x => x.Email == Email).FirstOrDefault();
                    user.Password = StringCipher.EncodePasswordToBase64(changePassword.ConfirmPassword);
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
