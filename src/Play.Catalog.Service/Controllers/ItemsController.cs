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

        public ItemDto GetById(Guid id) {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            return item;
        }
    }
}