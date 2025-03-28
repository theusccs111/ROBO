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
    public class RoboConfig : IEntityTypeConfiguration<Robo>
    {
        public void Configure(EntityTypeBuilder<Robo> builder)
        {
        }
    }
}