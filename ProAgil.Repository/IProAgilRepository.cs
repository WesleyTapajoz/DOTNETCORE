using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface IProAgilRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entityArray) where T : class;

        Task<bool> SaveChangesAsync();

        //Eventos
        Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes = false);
        Task<Evento[]> GetAllEventoAsync(bool includePalestrantes  = false);
        Task<Evento> GetAllEventoAsyncById(int eventoId, bool includePalestrantes = false);

        //Palestrante
        Task<Palestrante> GetPalestranteAsync(int palestranteId, bool includeEventos = false);
        Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false);
    }
}