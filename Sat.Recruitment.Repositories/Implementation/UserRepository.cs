using Microsoft.Extensions.Options;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Enums;
using Sat.Recruitment.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sat.Recruitment.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly IOptionsMonitor<Settings> _settingOptions;

        public UserRepository(IOptionsMonitor<Settings> settingOptions)
        {
            _settingOptions = settingOptions;
        }

        public async Task Add(User user)
        {
            string newUser = $"{user.Name},{user.Email},{user.Phone},{user.Address},{Enum.GetName(typeof(UserType), user.UserType)},{user.Money}";
            var path = Directory.GetCurrentDirectory() + _settingOptions.CurrentValue.CurrentFile;
            await File.AppendAllTextAsync(path, newUser + Environment.NewLine);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = new List<User>();
            using (StreamReader reader = new StreamReader(GetFile(FileMode.Open)))
            {
                while (reader.Peek() >= 0)
                {
                    var line = await reader.ReadLineAsync();
                    users.Add(new User
                    {
                        Name = line.Split(',')[0].ToString(),
                        Email = line.Split(',')[1].ToString(),
                        Phone = line.Split(',')[2].ToString(),
                        Address = line.Split(',')[3].ToString(),
                        UserType = (UserType)Enum.Parse(typeof(UserType), line.Split(',')[4].ToString()),
                        Money = decimal.Parse(line.Split(',')[5].ToString()),
                    });
                }
                return users;
            }
        }

        private FileStream GetFile(FileMode fileMode)
        {
            var path = Directory.GetCurrentDirectory() + _settingOptions.CurrentValue.CurrentFile;
            FileStream fileStream = new FileStream(path, fileMode);
            return fileStream;
        }
    }
}
