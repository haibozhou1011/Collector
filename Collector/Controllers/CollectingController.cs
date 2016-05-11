using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Collector.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Collector.Controllers
{
    public class CollectingController : Controller
    {
        // GET: Collecting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(CollectionViewModel collectionView)
        {
            collectionView = ColllectWithParse(collectionView);
            return View(collectionView);
        }

        public CollectionViewModel ColllectWithParse(CollectionViewModel collectionView)
        {
            if (collectionView == null || string.IsNullOrEmpty(collectionView.CollectionUrl))
            {
                return collectionView;
            }
            var client = new RestClient(collectionView.CollectionUrl);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var htmlContent = response.Content;
            collectionView.ProductViews = ParseProducts(htmlContent);
            return collectionView;
        }

        public IEnumerable<CollectedProductViewModel> ParseProducts(string productHtmlContent)
        {
            var productName = RegexMatchValue(ParseProductPatterns.ProductNamePattern, productHtmlContent);
            var productCuurency = RegexMatchValue(ParseProductPatterns.ProductCurrencyPattern, productHtmlContent);

            var productJson = RegexMatchValue(ParseProductPatterns.ProductJsnPattern, productHtmlContent);

            var prodctJsonArray = JArray.Parse(productJson);
            var products =
                prodctJsonArray.Select(pja =>
                {
                    var colorWithSizeCode = pja["skuPropIds"].ToString().Split(',');
                    var priceJson = pja["skuVal"];
                    var skuPrice = priceJson["skuPrice"];
                    var price = skuPrice == null ? "0" : skuPrice.ToString();
                    var actSkuPrice = priceJson["actSkuPrice"];
                    var discountPrice = actSkuPrice == null ? "0" : actSkuPrice.ToString();
                    return new
                    {
                        ColorCode = colorWithSizeCode.First(),
                        SizeCode = colorWithSizeCode.Last(),
                        Price = Convert.ToDecimal(price),
                        DiscountPrice = Convert.ToDecimal(discountPrice),
                    };
                }).ToList();

            var collectedImages = ParseProducImages(productHtmlContent);

            var collectedProducts = products.Select(p => new CollectedProductViewModel
            {
                ProductName = productName,
                ProductPrice = p.Price,
                ProductDiscountPrice = p.DiscountPrice,
                ProductCurrency = productCuurency,
                ProductColor = SetProductColorWithSize(ParseProductPatterns.ProductColorPattern,p.ColorCode,productHtmlContent),
                ProductSize = SetProductColorWithSize(ParseProductPatterns.ProductSizePattern, p.SizeCode, productHtmlContent),
                ProductImages = collectedImages
            }).ToList();
            return collectedProducts;
        }

        private IEnumerable<CollectedProductImageViewModel> ParseProducImages(string productHtmlContent)
        {
            var imagesJson = RegexMatchValue(ParseProductPatterns.ProductImageJsonPattern, productHtmlContent);
            var imageJsonArray = JArray.Parse(imagesJson);

            var images = imageJsonArray.ToObject<List<string>>();
            return images.Select((t, i) => new CollectedProductImageViewModel
            {
                ImageUrl = t,
                Sort = i
            });
        }

        private string SetProductColorWithSize(string pattern, string colorWithSizeCode,string input)
        {
            var newPattern = string.Format(pattern, colorWithSizeCode);
            return RegexMatchValue(newPattern, input);
        }

        private string RegexMatchValue(string pattern, string input, RegexOptions regexOptions = RegexOptions.IgnoreCase|RegexOptions.Singleline)
        {
            var regex = new Regex(pattern, regexOptions);
            var match = regex.Match(input);
            return match.Value;
        }
    }
}