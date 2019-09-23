namespace ProAgil.Domain
{
    public class RedeSocial
    {
        public int RedeSocialId { get; set; }
        public string Nome { get; set; }
        public string URL { get; set; }
        public int? EventoId { get; set; }
        public virtual Evento Evento { get; }
        public int? PalestranteId { get; set; }
        public virtual Palestrante Palestrante { get; }
    }
}