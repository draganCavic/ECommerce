using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.DTOs;
using OnlineStore.Entities;
using OnlineStore.Extensions;
using OnlineStore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper,
            IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();

            return Ok(users);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDto userUpdateDto)
        {

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(userUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto(IFormFile file)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photo != null)
            {
                var resultPhoto = await _photoService.DeletePhotoAsync(user.Photo.PublicId);
                if (resultPhoto.Error != null) return BadRequest(resultPhoto.Error.Message);
            }

            user.Photo = photo;

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, photo);
            }


            return BadRequest("Problem addding photo");
        }

        [HttpGet("{userId}/wishlist")]
        public async Task<ActionResult<IEnumerable<WishlistReturnDto>>> GetUserWishlist(int userId)
        {
            if (userId != User.GetUserId()) return Unauthorized();

            var product = await _unitOfWork.WishlistRepository.GetProductsAsync(userId);

            return Ok(product);
        }

        [HttpPost("wishlist/{productId}")]
        public async Task<ActionResult> AddProductToWishlist(int productId)
        {
            if (await _unitOfWork.ProductRepository.GetProductByIdAsync(productId) == null) return NotFound();

            if (await _unitOfWork.WishlistRepository.GetWishlist(User.GetUserId(), productId) != null) return BadRequest("You already added this product");

            _unitOfWork.WishlistRepository.AddProductOnWishlist(new Wishlist { ProductId = productId, UserId = User.GetUserId() });

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to add product");
        }

        [HttpDelete("wishlist/{productId}")]
        public async Task<ActionResult> RemoveProductFromWishlist(int productId)
        {
            if (await _unitOfWork.ProductRepository.GetProductByIdAsync(productId) == null) return NotFound();

            var wishlist = await _unitOfWork.WishlistRepository.GetWishlist(User.GetUserId(), productId);
            if (wishlist == null) return NotFound();

            _unitOfWork.WishlistRepository.DeleteProductFromWishlist(wishlist);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to remove product");
        }
    }
}
