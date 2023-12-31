using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers{

    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase 
    {
       private readonly IRepository<Item> itemsRepository;

       private static int requestCounter = 0;

       public ItemsController(IRepository<Item> itemsRepository)
       {
            this.itemsRepository = itemsRepository;
       }
       

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync() 
        {
            requestCounter++;
            Console.WriteLine($"Request {requestCounter}: Starting..");
            if (requestCounter <= 2)
            {
                   Console.WriteLine($"Request {requestCounter}: Delaying");
                   await Task.Delay(TimeSpan.FromSeconds(10));
            }

            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id) {
            var item = await itemsRepository.GetAsync(id);
            if (item == null) {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto) 
        {
            var item = new Item {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            
            await itemsRepository.CreateAsync(item);

            return CreatedAtAction(nameof(GetByIdAsync), new {id= item.Id}, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updatedItemDto) {
            
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null) {
                return NotFound();
            }

            existingItem.Name = updatedItemDto.Name;
            existingItem.Description = updatedItemDto.Description;
            existingItem.Price = updatedItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id) 
        {
             var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null) {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(existingItem.Id);

            return NoContent();
        }
    }
}