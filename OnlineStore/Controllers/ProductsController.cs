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
    public class ProductsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper,
            IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }
        //radi
        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByName(string name)
        {
            return Ok(await _unitOfWork.ProductRepository.GetProductByNameAsync(name));
        }
        //radi
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            return _mapper.Map<ProductDto>(product);
        }
        //radi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _unitOfWork.ProductRepository.GetProductsAsync());
        }
        //radi
        [HttpGet("users/{userId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetUserProducts(int userId)
        {
            var product = await _unitOfWork.ProductRepository.GetUserProductsAsync(userId);

            return Ok(product);
        }
        //radi
        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductCreateDto productCreateDto)
        {
            _unitOfWork.ProductRepository.CreateProduct(productCreateDto, User.GetUserId());

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to create product");
        }
        //radi
        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(int productId, ProductCreateDto productUpdateDto)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            if (User.GetUserId() != product.UserId) return Unauthorized();

            _mapper.Map(productUpdateDto, product);

            _unitOfWork.ProductRepository.UpdateProduct(product);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update product");
        }
        //radi
        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            if (User.GetUserId() != product.UserId) return Unauthorized();

            _unitOfWork.ProductRepository.DeleteProduct(product);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to delete product");
        }
        //mrziii me
        [HttpPost("{id}/add-photo")]
        public async Task<ActionResult<Photo>> AddPhoto(int id, IFormFile file)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);

            if (User.GetUserId() != product.UserId) return Unauthorized();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            product.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                return CreatedAtRoute("GetProduct", new { id = product.Id }, photo);
            }


            return BadRequest("Problem addding photo");
        }
        //haahahahaaha
        [HttpDelete("{productId}/delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int productId, int photoId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);

            var photo = product.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            product.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
        //radi
        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<ReviewDto>> GetReviews(int productId)
        {
            if (await _unitOfWork.ProductRepository.GetProductByIdAsync(productId) == null) return NotFound();

            return Ok(await _unitOfWork.ReviewRepository.GetProductReviews(productId));
        }
        //radi
        [HttpGet("{productId}/reviews/{userId}")]
        public async Task<ActionResult<ReviewDto>> GetReview(int productId, int userId)
        {
            if (await _unitOfWork.ProductRepository.GetProductByIdAsync(productId) == null) return NotFound();

            if (await _unitOfWork.UserRepository.GetUserByIdAsync(userId) == null) return NotFound();

            var review = await _unitOfWork.ReviewRepository.GetProductReview(productId, userId);
            return Ok(_mapper.Map<ReviewDto>(review));
        }
        //radi
        [HttpPost("{productId}/reviews")]
        public async Task<ActionResult> PostReview(int productId, ReviewCreateDto reviewDto)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null) return NotFound();
            if (await _unitOfWork.ReviewRepository.GetProductReview(productId, User.GetUserId()) != null) return BadRequest("You have already posted a review");

            var review = _mapper.Map<Review>(reviewDto);
            review.ProductId = productId;
            review.UserId = User.GetUserId();

            var count = (await _unitOfWork.ReviewRepository.GetProductReviews(productId)).Count();
            product.AverageRating = ((product.AverageRating * count) + reviewDto.Rate) / (count + 1);

            _unitOfWork.ReviewRepository.AddReview(review);
            _unitOfWork.ProductRepository.UpdateProduct(product);
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to post review");
        }
        //radi
        [HttpDelete("{productId}/reviews/{userId}")]
        public async Task<ActionResult> RemoveReview(int productId, int userId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null) return NotFound();

            if (User.GetUserId() != userId) return Unauthorized();

            var review = await _unitOfWork.ReviewRepository.GetProductReview(productId, userId);
            if (review == null) return NotFound();

            var count = (await _unitOfWork.ReviewRepository.GetProductReviews(productId)).Count();
            if (count == 1) product.AverageRating = 0;
            else product.AverageRating = ((product.AverageRating * count) - review.Rate) / (count - 1);

            _unitOfWork.ReviewRepository.RemoveReview(review);
            _unitOfWork.ProductRepository.UpdateProduct(product);
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to remove review");
        }
    }
}
