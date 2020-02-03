using DATA.Domain.Models;
using DATA.Domain.Supervisor;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TESTE.Domain
{
    [TestClass]
    public class UserTeste
    {
        private AppDbContext _ctx;
        private User _LocalUser;
        private UserSupervisor _usr;

        public UserTeste()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0")
                .Options;
            _ctx = new AppDbContext(optionsBuilder);
            _usr = new UserSupervisor(this._ctx);
        }

        [TestInitialize]
        public void CreateAndFind()
        {
            //Valida a criacao e o get de um usuario
            Random rnd = new Random();
            _LocalUser = new User
            {
                Nome = "Teste Automatizado",
                Email = String.Format("{0}@{1}.com", rnd.Next(), rnd.Next()),
                Senha = "teste"
            };
            _usr.Add(_LocalUser);
            _LocalUser = _usr.Find(_LocalUser.Email);
            Assert.AreNotEqual(_LocalUser.Id, 0);
        }
        [TestMethod]
        public void AlterarSenha()
        {
            //Valida alteracao de senha e login do usuario
            _usr.UpdatePwd(_LocalUser.Id, "root");
            Assert.AreEqual(_usr.ValidUser(new DATA.Domain.Views.ViewLogin { Usuario = _LocalUser.Email, Senha = "root" }), true);
        }
        [TestMethod]
        public void AlterarUsuario()
        {
            //Valida alteracao do usuario
            _LocalUser.Nome = "Teste Alterado";
            _usr.Update(_LocalUser);
            Assert.AreEqual(_usr.Find(_LocalUser.Id).Nome, _LocalUser.Nome);
        }

        [TestCleanup]
        public void Delete()
        {
            Assert.AreEqual(true, _usr.Remove(_LocalUser.Id));
        }
    }
}
