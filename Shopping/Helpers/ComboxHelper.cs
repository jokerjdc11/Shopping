using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;

namespace Shopping.Helpers
{
	public class ComboxHelper : IComboxHelper
	{
		private readonly DataContext _context;

		public ComboxHelper(DataContext context)
        {
			_context = context;
		}

        public async Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync()
		{
			// se devuelve una lista de categorias ordenada por text 
			List<SelectListItem> list = await _context.Categories.Select (c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			})
				.OrderBy(c => c.Text)
				.ToListAsync();

			list.Insert(0, new SelectListItem { Text = "[Seleccione una categoría....", Value = "0" });

			return list;
		}

		public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
		{
			// se devuelve una lista de categorias ordenada por text 
			List<SelectListItem> list = await _context.Cities
				.Where(s => s.State.Id == stateId)
				.Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				})
				.OrderBy(c => c.Text)
				.ToListAsync();

			list.Insert(0, new SelectListItem { Text = "[Seleccione una ciudad....", Value = "0" });

			return list;
		}

		public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
		{
			// se devuelve una lista de categorias ordenada por text 
			List<SelectListItem> list = await _context.Countries.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			})
				.OrderBy(c => c.Text)
				.ToListAsync();

			list.Insert(0, new SelectListItem { Text = "[Seleccione una país....", Value = "0" });

			return list;
		}

		public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
		{
			// se devuelve una lista de categorias ordenada por text 
			List<SelectListItem> list = await _context.States
				.Where(s => s.Country.Id == countryId)
				.Select(c => new SelectListItem
			{
				Text = c.Name,
				Value = c.Id.ToString()
			})
				.OrderBy(c => c.Text)
				.ToListAsync();

			list.Insert(0, new SelectListItem { Text = "[Seleccione una Departamento/estado....", Value = "0" });

			return list;
		}
	}
}
