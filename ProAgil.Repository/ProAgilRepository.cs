using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;

        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        void IProAgilRepository.Add<T>(T entity)
        {
            _context.Add(entity);
        }

        void IProAgilRepository.Update<T>(T entity)
        {
            _context.Update(entity);
        }

        void IProAgilRepository.Delete<T>(T entity)
        {
            _context.Remove(entity);
        }

        async Task<bool> IProAgilRepository.SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        async Task<Evento[]> IProAgilRepository.GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(o => o.DataEvento);
            return await query.ToArrayAsync();
        }
        async Task<Evento[]> IProAgilRepository.GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(o => o.DataEvento).Where(s => s.Tema.ToLower().Contains(tema.ToLower()));
            return await query.ToArrayAsync();
        }

        async Task<Evento> IProAgilRepository.GetAllEventoAsyncById(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos.Include(x => x.Lotes).Include(x => x.RedesSociais);
            if (includePalestrantes)
            {
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Palestrante);
            }
            query = query.AsNoTracking().OrderByDescending(o => o.DataEvento).Where(s => s.EventoId == eventoId);
            return await query.FirstOrDefaultAsync();
        }

        async Task<Palestrante> IProAgilRepository.GetPalestranteAsync(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(x => x.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Evento);
            }
            query = query.AsNoTracking().OrderByDescending(o => o.Nome).Where(s => s.PalestranteId == palestranteId);
            return await query.FirstOrDefaultAsync();
        }

        async Task<Palestrante[]> IProAgilRepository.GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrantes.Include(x => x.RedesSociais);
            if (includeEventos)
            {
                query = query.Include(x => x.PalestranteEventos).ThenInclude(x => x.Evento);
            }
            query = query.AsNoTracking().Where(s => s.Nome.ToLower().Contains(name.ToLower()));
            return await query.ToArrayAsync();
        }
    }
}