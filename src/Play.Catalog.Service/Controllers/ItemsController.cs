using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers{

    [ApiController]
    [Route("items")]
    public class ItemController : ControllerBase 
    {
        private static readonly List<ItemDto> items = new() {
            new ItemDto(Guid.NewGuid(), "Potion", "Restore health", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidoe", "Cure poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Sword", "Deals damage", 20, DateTimeOffset.UtcNow), 
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get() 
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id) {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if (item == null) {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto) 
        {
            bool itemIsAlreadyCreated = items.Exists(item => item.Name == createItemDto.Name);

            if (itemIsAlreadyCreated) {
                return BadRequest("An item with the same name already exists.");
            }
            var item = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);

            return CreatedAtAction(nameof(GetById), new {id= item.Id}, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto updatedItemDto) {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            
            if (existingItem == null) {
                return NotFound();
            }
            var updatedItem = existingItem with{
                Name = updatedItemDto.Name,
                Description = updatedItemDto.Description,
                Price = updatedItemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id) 
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            if (index == -1 ) {
                return NotFound();
            }
            items.RemoveAt(index);

            return NoContent();
        }
    }
}