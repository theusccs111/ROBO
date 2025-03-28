using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ROBO.Dominio.Entidades;

namespace ROBO.Persistencia.EntityConfig
{
    public class LogConfig : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(p => p.Descricao).HasMaxLength(1000);
        }
    }
}
