using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.Dtos
{
    public class EventoDto
    {
        public int EventoId { get; set; }
        [Required(ErrorMessage="Obrigatório")]
        [StringLength(100, MinimumLength=3, ErrorMessage="Local é entre 3 e 100 caracteres")]
        public string  Local { get; set; }
        public string DataEvento { get; set; }
        [Required (ErrorMessage="Obrigatório")]
        public string Tema { get; set; }
        [Range(1, 50, ErrorMessage="Quantidade de pessoas minimo 1 máximo 50.")]
        public int QtdPessoas { get; set; }
        public string ImagemURL { get; set; }
        [Phone]
        public string Telefone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }

    }
}