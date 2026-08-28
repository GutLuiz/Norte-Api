namespace UepaMed.Domain.Entities
{
    public class DuplicidadeIgnorada
    {
        public int Id { get; set; }

        public int RevisaoId { get; set; }

        public int ArtigoAId { get; set; }

        public int ArtigoBId { get; set; }

        public DateTime DataDecisao { get; set; }
    }
}