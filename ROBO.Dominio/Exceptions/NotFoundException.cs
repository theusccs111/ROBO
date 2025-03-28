using System;
using System.Collections.Generic;
using System.Text;

namespace ROBO.Dominio.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"Entidade \"{name}\" ({key}) não encontrada.")
        {
        }

        public NotFoundException(string name) : base($"{name}")
        {

        }
    }
}
