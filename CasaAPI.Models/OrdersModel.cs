using CasaAPI.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasaAPI.Models
{
    public class OrderSaveParameters
    {
        public OrderSaveParameters()
        {
            orderDetailsList = new List<OrderDetailsSaveParameters>();
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public int StateId { get; set; }
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
        public int AreaId { get; set; }

        [DefaultValue("")]
        public string Pincode { get; set; }
        public int BrandId { get; set; }
        public int PanelQty { get; set; }
        public int BinderQty { get; set; }

        [DefaultValue("")]
        public string Remarks { get; set; }
        public int GrandTotalQty { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public List<OrderDetailsSaveParameters> orderDetailsList { get; set; }
    }
    public class OrderDetailsSaveParameters
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CollectionId { get; set; }
        public int BaseDesignId { get; set; }
        public int SizeId { get; set; }
        public int SurfaceId { get; set; }
        public int ThicknessId { get; set; }
        public int Quantity { get; set; }
    }
    public class OrderSearchParameters
    {
        public PaginationParameters pagination { get; set; }

        [DefaultValue("")]
        public string ValueForSearch { get; set; } = null;
        public int CustomerId { get; set; }
        public int StatusId { get; set; }

        [DefaultValue("")]
        public string BrandId { get; set; }

        [DefaultValue("")]
        public string CollectionId { get; set; }

        [DefaultValue("")]
        public string BaseDesignId { get; set; }

        [DefaultValue("")]
        public string SizeId { get; set; }
        public bool? IsActive { get; set; }
    }
    public class OrderListResponse
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string EmailId { get; set; }
        public string RefName { get; set; }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public string Pincode { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public int PanelQty { get; set; }
        public int BinderQty { get; set; }
        public string Remarks { get; set; }
        public int GrandTotalQty { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public bool IsActive { get; set; }
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class OrderDetailsSearchParameters
    {
        public PaginationParameters pagination { get; set; }

        [DefaultValue("")]
        public string ValueForSearch { get; set; } = null;
        public int OrderId { get; set; }
    }

    public class OrderDetailsByIdResponse
    {
        public OrderDetailsByIdResponse()
        {
            orderDetails = new List<OrderDetailsResponse>();
        }
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }

        public int CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string MobileNo { get; set; }
        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public string EmailId { get; set; }
        public string RefName { get; set; }

        public int StateId { get; set; }
        public string StateName { get; set; }

        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int AreaId { get; set; }
        public string AreaName { get; set; }

        public string Pincode { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public int PanelQty { get; set; }
        public int BinderQty { get; set; }
        public string Remarks { get; set; }
        public int GrandTotalQty { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public bool IsActive { get; set; }
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<OrderDetailsResponse> orderDetails { get; set; }
    }
    public class OrderDetailsResponse
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string OrderNo { get; set; }
        public int CollectionId { get; set; }
        public string CollectionName { get; set; }
        public int BaseDesignId { get; set; }
        public string BaseDesignName { get; set; }
        public int SizeId { get; set; }
        public string SizeName { get; set; }
        public int SurfaceId { get; set; }
        public string SurfaceName { get; set; }
        public int ThicknessId { get; set; }
        public string ThicknessName { get; set; }
        public int Quantity { get; set; }
        public string CreatorName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
