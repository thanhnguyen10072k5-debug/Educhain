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
    // Lưu trữ danh sách điểm theo Mã sinh viên
    mapping(string => GradeRecord[]) private studentGrades;

    // Sự kiện được phát ra mỗi khi có điểm mới được thêm vào
    event GradeAdded(string indexed studentCode, string subjectCode, uint16 score, string semester, uint256 timestamp);

    // Chỉ cho phép Admin/Giảng viên mới có quyền thêm điểm
    modifier onlyAdmin() {
        require(msg.sender == admin, "Chi co Admin hoac Giang vien moi co quyen nhap diem!");
        _;
    }
    // Hàm khởi tạo, gán quyền admin cho người triển khai hợp đồng
    constructor() {
        admin = msg.sender;
    }

    // Hàm thêm điểm số lên Blockchain
    function addGrade(string memory _studentCode, string memory _subjectCode, uint16 _score, string memory _semester) public onlyAdmin {
        GradeRecord memory newGrade = GradeRecord({
            studentCode: _studentCode,
            subjectCode: _subjectCode,
            score: _score,
            semester: _semester,
            timestamp: block.timestamp
        });

        studentGrades[_studentCode].push(newGrade);
        
        emit GradeAdded(_studentCode, _subjectCode, _score, _semester, block.timestamp);
    }

    // Hàm tra cứu lịch sử điểm của một sinh viên (Không tốn phí gas)
    function getGradesByStudent(string memory _studentCode) public view returns (GradeRecord[] memory) {
        return studentGrades[_studentCode];
    }
}