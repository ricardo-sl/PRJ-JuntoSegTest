using DATA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA.Domain.Seeds
{
    public class InitialDebugSeeder
    {
        private Random gen = new Random();
        private string[] _nomes = "Joao,Gabriel,Lucas,Pedro,Mateus,Jose,Gustavo,Guilherme,Carlos,Vitor,Felipe,Marcos,Rafael,Luiz,Daniel,Eduardo,Matheus,Luis,Bruno,Paulo,Maria,Ana,Vitoria,Julia,Leticia,Amanda,Beatriz,Larissa,Gabriela,Mariana,Bruna,Camila,Isabela,Luana,Sara,Eduarda,Bianca,Rafaela,Geovana,Fernanda".Split(',');
        private string[] _emails = "gmail,uol,bol,yahoo,hotmail,aol,msn".Split(',');
        string _letrasParaSenha = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
        private AppDbContext _ctx;
        private int maxRandom = 99999;
        public InitialDebugSeeder(AppDbContext appDBContext)
        {
            _ctx = appDBContext;
        }

        public void SeedData()
        {
            //_ctx.Users.RemoveRange(_ctx.Users.ToList());
            //_ctx.SaveChanges();
            //Se estivermos debugando e o bd estiver vazio, o seed será feito
#if DEBUG
            if (_ctx.Users.Count() == 0 && System.Diagnostics.Debugger.IsAttached)
            { 
                //user padrao para testes
                User _usr = new User();
                _usr.Nome = "Admin";
                _usr.Email = "admin@admin.com";
                _usr.Senha = "root";
                _usr.EncriptarSenha();
                _ctx.Add(_usr); 
                for (int i = 0; i < 45; i++)
                {
                    _usr = new User(); 
                    _usr.Nome = String.Format("{0} {1}", _nomes[gen.Next(_nomes.Length - 1)], _nomes[gen.Next(_nomes.Length - 1)]);
                    _usr.Email = String.Format("{0}@{1}.com", _usr.Nome.Replace(' ', '.'), _emails[gen.Next(_emails.Length - 1)]).ToLower();
                    _usr.Senha = GerarSenha();
                    _usr.EncriptarSenha();
                    try
                    {
                        _ctx.Add(_usr); 
                    }catch (Exception){}
                } 
                _ctx.SaveChanges();
            }
#endif
        } 
        public string GerarSenha()
        {
            char[] chars = new char[8];
            Random rd = new Random();
            for (int i = 0; i < 8; i++)
            {
                chars[i] = _letrasParaSenha[rd.Next(0, _letrasParaSenha.Length)];
            }
            return new string(chars);
        }
    }
}
