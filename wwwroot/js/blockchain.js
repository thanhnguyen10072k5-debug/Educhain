// 1. Dán địa chỉ Contract 
const contractAddress = "0xd8b934580fcE35a11B58C6D73aDeE468a2833fa8";

// 2. dán  đoạn code ABI
const contractABI = [
	{
		"inputs": [],
		"stateMutability": "nonpayable",
		"type": "constructor"
	},
	{
		"anonymous": false,
		"inputs": [
			{
				"indexed": true,
				"internalType": "string",
				"name": "maSinhVien",
				"type": "string"
			},
			{
				"indexed": false,
				"internalType": "string",
				"name": "maMonHoc",
				"type": "string"
			},
			{
				"indexed": false,
				"internalType": "uint16",
				"name": "diemSo",
				"type": "uint16"
			},
			{
				"indexed": false,
				"internalType": "string",
				"name": "hocKy",
				"type": "string"
			},
			{
				"indexed": false,
				"internalType": "uint256",
				"name": "thoiGian",
				"type": "uint256"
			}
		],
		"name": "DiemDuocThem",
		"type": "event"
	},
	{
		"inputs": [],
		"name": "admin",
		"outputs": [
			{
				"internalType": "address",
				"name": "",
				"type": "address"
			}
		],
		"stateMutability": "view",
		"type": "function"
	},
	{
		"inputs": [
			{
				"internalType": "string",
				"name": "_maSinhVien",
				"type": "string"
			}
		],
		"name": "layDiemTheoMaSV",
		"outputs": [
			{
				"components": [
					{
						"internalType": "string",
						"name": "maSinhVien",
						"type": "string"
					},
					{
						"internalType": "string",
						"name": "maMonHoc",
						"type": "string"
					},
					{
						"internalType": "uint16",
						"name": "diemSo",
						"type": "uint16"
					},
					{
						"internalType": "string",
						"name": "hocKy",
						"type": "string"
					},
					{
						"internalType": "uint256",
						"name": "thoiGian",
						"type": "uint256"
					}
				],
				"internalType": "struct EduChainGrade.BanGhiDiem[]",
				"name": "",
				"type": "tuple[]"
			}
		],
		"stateMutability": "view",
		"type": "function"
	},
	{
		"inputs": [
			{
				"internalType": "string",
				"name": "_maSinhVien",
				"type": "string"
			},
			{
				"internalType": "string",
				"name": "_maMonHoc",
				"type": "string"
			},
			{
				"internalType": "uint16",
				"name": "_diemSo",
				"type": "uint16"
			},
			{
				"internalType": "string",
				"name": "_hocKy",
				"type": "string"
			}
		],
		"name": "themDiem",
		"outputs": [],
		"stateMutability": "nonpayable",
		"type": "function"
	}
];

// 3. Viết hàm Kết nối ví MetaMask
async function connectMetaMask() {
    // Kiểm tra xem trình duyệt đã cài MetaMask chưa
    if (typeof window.ethereum !== 'undefined') {
        try {
            // Yêu cầu kết nối
            const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
            const account = accounts[0];
            
            // Thông báo thành công và thu gọn địa chỉ ví trên nút bấm
            alert("Kết nối thành công! Ví của bạn: " + account);
            document.getElementById('btnConnect').innerText = account.substring(0, 6) + "..." + account.substring(38);
            
        } catch (error) {
            console.error("Lỗi kết nối:", error);
            alert("Bạn đã từ chối kết nối hoặc có lỗi xảy ra.");
        }
    } else {
        alert("Vui lòng cài đặt tiện ích mở rộng MetaMask trên trình duyệt!");
    }
}