using map.backend.shared.Entities.Auth;
using map.backend.shared.Interfaces.UnitOfWork;
using map.backend.shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map.backend.shared.Migrations.Seeder
{
    public static class SeedData
    {
        public static void Seed(AppDbContext dbContext)
        {
            string userId = "admin";
            string password = "123456";
            string passwordhash = BCrypt.Net.BCrypt.HashPassword(password);

            var checkUser = dbContext.tb_user.FirstOrDefault();
            if (checkUser == null)
            {
                tb_user _user = new tb_user();
                _user.userid = userId;
                _user.username = "Quản trị viên";
                _user.password = passwordhash;
                _user.email = "admin@gamil.com";
                _user.phone = "0368444555";
                _user.record_stat = "O";
                _user.create_by = "AUTO";
                _user.create_date = DateTime.Now;
                dbContext.tb_user.Add(_user);
                dbContext.SaveChanges();
            }
        }
    }
}
