using DATA.Domain.Models;
using DATA.Domain.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.Domain.Supervisor
{
    public class UserSupervisor
    {
        private AppDbContext _ctx;
        public UserSupervisor(AppDbContext appDBContext)
        {
            _ctx = appDBContext;
        }

        public List<User> toList(ref ViewPagination pag)
        {
            pag.Count = _ctx.Users.Count();
            return _ctx.Users.Skip(pag.StartAt()).Take(pag.OffSet).AsNoTracking().ToList();
        }
        public List<User> FindLike(string Nome, string Email="")
        {
            return _ctx.Users.Where(w =>( EF.Functions.Like(w.Nome.ToLower().Trim(), string.Format("%{0}%", Nome.ToLower().Trim())) && Nome.Trim() != "") || (EF.Functions.Like(w.Email.ToLower().Trim(), string.Format("%{0}%", Email.ToLower().Trim())) && Email.Trim() != "")).AsNoTracking().ToList();
        }
        public User Find(int Id)
        {
            return _ctx.Users.Find(Id);
        }
        public User Find(string Email)
        {
            return _ctx.Users.Where(w => w.Email == Email).FirstOrDefault();
        }
        public bool Remove(int Id)
        {
            _ctx.Remove(_ctx.Users.Find(Id));
            return _ctx.SaveChanges() != 0;
        }
        public int Add(User usr)
        {
            usr.EncriptarSenha();
            _ctx.Add(usr);
            return _ctx.SaveChanges();
        }

        public bool ValidUser(ViewLogin lgn)
        { 
            return _ctx.Users.Where(w => w.Email == lgn.Usuario && w.Senha == User.EncriptarSenha(lgn.Senha)).Count().Equals(1);
        }

        public void UpdatePwd(int id, string pwd)
        {
            User _luser = Find(id);
            _luser.Senha = pwd;
            Update(_luser);
        }
        public int Update(User usr)
        {
            User tmpUser = _ctx.Users.Where(w => w.Id == usr.Id).AsNoTracking().FirstOrDefault();
            if (tmpUser.Senha != User.EncriptarSenha(usr.Senha))
                usr.EncriptarSenha();
            _ctx.Entry(usr).State = EntityState.Modified;
            _ctx.Update(usr);
            return _ctx.SaveChanges();
        }
    }
}
