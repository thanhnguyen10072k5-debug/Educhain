pragma solidity ^0.8.19;

contract EduChainGrade {
    // Địa chỉ ví của người quản lý (Admin/Giảng viên) triển khai hợp đồng
    address public admin;

    // Cấu trúc dữ liệu của một bảng điểm
    struct GradeRecord {
        string studentCode;  // Mã sinh viên
        string subjectCode;  // Mã môn học
        uint16 score;        // Điểm số (nhân 10 hoặc 100 để lưu số thập phân, ví dụ 8.5 lưu thành 85)
        string semester;     // Học kỳ
        uint256 timestamp;   // Thời gian ghi nhận lên chuỗi
    }