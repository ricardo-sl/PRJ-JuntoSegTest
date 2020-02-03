using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace DATA.Domain.Models
{
    public class User : BaseEntity
    {
        //A Classe já trara algumas regras basicas de validacao de cadastro
        [Required(ErrorMessage = "Preenchimento do nome é obrigatório")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Preenchimento do email é obrigatório")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        //a linha abaixo acabou ficando inutil depois de transformar a senha em md5
        [StringLength(32 , MinimumLength = 6, ErrorMessage = "Senha deve ter entre 6 e 32 caracteres")]
        public string Senha { get; set; }


        public static string EncriptarSenha(string Senha)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Senha));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            { sBuilder.Append(data[i].ToString("x2")); }
            return sBuilder.ToString();
        }
        public void EncriptarSenha()
        {
            Senha = EncriptarSenha(Senha);
        }
    }
}
