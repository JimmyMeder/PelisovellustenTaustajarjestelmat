using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Assignment2.Controllers
{
    [ApiController]
    [Route("player/{playerId}/items")]
    public class ItemsController : ControllerBase
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IRepository _irepository;

        public ItemsController(ILogger<ItemsController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }

        [HttpPost]
        [Route("newItem/{id:Guid}")]
        public void new_item(Guid playerId, [FromBody] Item item)
        {
            Item newItem = item;
            Create(playerId, newItem);
        }


        public Item Create(Guid playerId, Item newItem)
        {
            _logger.LogInformation("Creating new item with name " + newItem.Name);
            Item item = new Item()
            {
                Id = Guid.NewGuid(),
                Name = newItem.Name,
                Level = newItem.Level,
                type = newItem.type
            };
            _logger.LogInformation("Name now: " + item.Name);
            _logger.LogInformation("ID now: " + item.Id);
            _irepository.CreateItem(playerId, item);
            return item;
        }

        [NotImplExceptionFilter]
        [HttpGet]
        [Route("ListItems")]
        public Task<Item[]> GetAllItems(Guid playerId)
        {
            Task<Item[]> list_items = _irepository.GetAllItems(playerId);
            return list_items;
        }

        [HttpPost]
        [Route("modItem/{id:Guid}")]
        public async Task<Player> Modify(Guid id, [FromBody] Item item)
        {
            await _irepository.UpdateItem(id, item);
            return null;
        }

        [HttpPost]
        [Route("deleteItem/{id:Guid}")]
        public async Task<Player> DeleteItem(Guid id, [FromBody] Item item)
        {
            await _irepository.DeleteItem(id, item);
            return null;
        }
    }

}