namespace FastFood.Core.Controllers
{
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Positions;

    public class PositionsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public PositionsController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreatePositionInputModel model)
        {
            if(string.IsNullOrWhiteSpace(model.PositionName))
                return View(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var position = this.mapper.Map<Position>(model);
            var positionName = context.Positions.Select(x => x.Name.ToLower()).ToList();
            
            if (positionName.Contains(position.Name.ToLower()))
            {
                return RedirectToAction("Position", new {name = position.Name });
            }

            Save(position);
            return this.RedirectToAction("All", "Positions");
        }

        private void Save(Position position)
        {
            this.context.Positions.Add(position);
            this.context.SaveChanges();
        }

        public IActionResult All()
        {
            var positions = this.context.Positions
                .ProjectTo<PositionsAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(positions);
        }

        public IActionResult Position(string name)
        {
            var positions = this.context.Positions
                .ProjectTo<PositionsPositionViewModel>(mapper.ConfigurationProvider)
                .Where(x => x.Name == name)
                .ToList();
            
            var position = new PositionsPositionViewModel();
            foreach (var errorModel in positions)
            {
                position.Id = errorModel.Id;
                position.Name = errorModel.Name;
            }
            
            return View(position);
        }
    }
}
