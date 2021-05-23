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
        //radi
        [HttpGet("username/{username}", Name = "GetUserByName")]
        public async Task<ActionResult<UserReturnDto>> GetUserByName(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<UserReturnDto>(user);
        }
        //radi
        [HttpGet("email/{email}", Name = "GetUserByEmail")]
        public async Task<ActionResult<UserReturnDto>> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            return _mapper.Map<UserReturnDto>(user);
        }
        //radi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserReturnDto>>> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync();

            return Ok(users);
        }
        //radi
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            if (User.GetUserId() != id) return Unauthorized();

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            _mapper.Map(userUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }
        //mrzi me
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
        //radi
        [HttpGet("wishlist")]
        public async Task<ActionResult<IEnumerable<WishlistReturnDto>>> GetUserWishlist()
        {
            return Ok(await _unitOfWork.WishlistRepository.GetProductsAsync(User.GetUserId()));
        }
        //radi
        [HttpPost("wishlist/{productId}")]
        public async Task<ActionResult> AddProductToWishlist(int productId)
        {
            if (await _unitOfWork.ProductRepository.GetProductByIdAsync(productId) == null) return NotFound();

            if (await _unitOfWork.WishlistRepository.GetWishlist(User.GetUserId(), productId) != null) return BadRequest("You already added this product");

            _unitOfWork.WishlistRepository.AddProductOnWishlist(new Wishlist { ProductId = productId, UserId = User.GetUserId() });

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to add product");
        }
        //radi
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
