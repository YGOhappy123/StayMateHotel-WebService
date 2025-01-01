using System;
using System.Collections.Generic;
using server.Models;

namespace server.Dtos.Service
{
    public class ServiceDto
    {
        public int Id { get; set; } // ID của dịch vụ
        public string Name { get; set; } = string.Empty; // Tên dịch vụ
        public decimal Price { get; set; } // Giá của dịch vụ
        public bool IsAvailable { get; set; } // Trạng thái dịch vụ có sẵn hay không
        public DateTime CreatedAt { get; set; } = DateTime.Now; // Thời gian tạo dịch vụ
        public int? CreatedById { get; set; } // ID người tạo dịch vụ
        public UserInfo? CreatedBy { get; set; } // Thông tin của người tạo (Admin)

        // Đây có thể là các mối quan hệ khác, ví dụ, danh sách BookingService nếu cần
        public List<BookingServiceInfo> BookingServices { get; set; } = new List<BookingServiceInfo>();
    }

    public class UserInfo
    {
        public int Id { get; set; } // ID của Admin
        public string FirstName { get; set; } = string.Empty; // Tên admin
        public string LastName { get; set; } = string.Empty; // Họ admin
        public string Email { get; set; } = string.Empty; // Email admin
    }

    public class BookingServiceInfo
    {
        public int Id { get; set; } // ID của BookingService
        public DateTime BookingDate { get; set; } // Ngày đặt dịch vụ
        public int Quantity { get; set; } // Số lượng dịch vụ được đặt
    }
}
