// 1. Dán địa chỉ Contract 
const contractAddress = "0x05dc53762347c35cb793bb6657f2075762c32040";

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
			// Cập nhật giao diện Trạng thái Blockchain khi kết nối ví thành công
            document.getElementById('txt-chain-title').innerText = "Mạng lưới hoạt động";
            document.getElementById('txt-chain-title').style.color = "#15945b";
            document.getElementById('txt-chain-subtitle').innerText = "Đã kết nối Smart Contract";
            document.getElementById('dot-chain-status').style.background = "#19b66c";
            document.getElementById('dot-chain-status').parentElement.style.background = "#e9f9f0";

            // Cập nhật địa chỉ MetaMask
            document.getElementById('txt-wallet-address').innerText = account.substring(0, 6) + "..." + account.substring(38);
            document.getElementById('status-wallet').innerText = "Đã kết nối";
            document.getElementById('status-wallet').style.color = "#16a36a";

            // Cập nhật địa chỉ Contract
            document.getElementById('txt-contract-address').innerText = contractAddress.substring(0, 6) + "..." + contractAddress.substring(38);
            document.getElementById('status-contract').innerText = "Đã kết nối";
            document.getElementById('status-contract').style.color = "#16a36a";
            document.getElementById('btnConnect').innerText = account.substring(0, 6) + "..." + account.substring(38);
            
        } catch (error) {
            console.error("Lỗi kết nối:", error);
            alert("Bạn đã từ chối kết nối hoặc có lỗi xảy ra.");
        }
    } else {
        alert("Vui lòng cài đặt tiện ích mở rộng MetaMask trên trình duyệt!");
    }
}
// 4. Hàm thực hiện ghi điểm lên Blockchain (dùng cho Giảng viên)
async function themDiemLenBlockchain(maSV, maMon, diem, hocKy) {
    try {
        // Yêu cầu Ethers.js kết nối với MetaMask
        const provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = provider.getSigner(); // Lấy tài khoản ví đang đăng nhập
        
        // Khởi tạo đối tượng hợp đồng
        const contract = new ethers.Contract(contractAddress, contractABI, signer);

        console.log("Đang gửi giao dịch lên Blockchain...");
        
        // Gọi hàm themDiem từ Smart Contract
        const tx = await contract.themDiem(maSV, maMon, diem, hocKy);
        
        // Chờ mạng lưới xác nhận khối
        await tx.wait();
		// Cập nhật giao diện Lịch sử giao dịch
        const txHash = tx.hash;
        document.getElementById('txt-tx-hash').innerText = txHash.substring(0, 10) + "..." + txHash.substring(58);
        document.getElementById('txt-tx-hash').style.color = "#347fe4";
        document.getElementById('status-tx').innerText = "Thành công";
        document.getElementById('status-tx').style.color = "#16a36a";
        alert("🎉 Đã lưu điểm thành công lên Blockchain! Dữ liệu không thể bị thay đổi.");
        
    } catch (error) {
        console.error("Lỗi khi thêm điểm:", error);
        alert("Có lỗi xảy ra khi đẩy lên Blockchain. Vui lòng thử lại!");
    }
}
async function traCuuDiemTuBlockchain() {
    const maSV = document.getElementById("txtSearchMaSV").value.trim();
    if (!maSV) {
        alert("Vui lòng nhập Mã Sinh Viên cần tra cứu!");
        return;
    }

    const tbody = document.getElementById("bangDuLieuBlockchain");
    tbody.innerHTML = `<tr><td colspan="5" style="text-align: center; padding: 20px;">⏳ Đang truy xuất dữ liệu từ mạng Sepolia...</td></tr>`;

    try {
        // Đọc dữ liệu chỉ cần Provider (không cần Signer hay xác nhận MetaMask)
        const provider = new ethers.providers.Web3Provider(window.ethereum);
        const contract = new ethers.Contract(contractAddress, contractABI, provider);

        // Gọi hàm layDiemTheoMaSV từ Smart Contract
        const danhSachDiem = await contract.layDiemTheoMaSV(maSV);

        // Xóa thông báo Đang tải
        tbody.innerHTML = "";

        if (danhSachDiem.length === 0) {
            tbody.innerHTML = `<tr><td colspan="5" style="text-align: center; padding: 20px; color: #e11d48;">Chưa có dữ liệu điểm nào của sinh viên ${maSV} trên Blockchain.</td></tr>`;
            return;
        }

        // Duyệt mảng điểm trả về và nhét vào bảng HTML
        danhSachDiem.forEach((diem, index) => {
            const tr = document.createElement("tr");
            tr.style.borderBottom = "1px solid #f1f5f9";
            
            // Lưu ý: Các biến diem.maMonHoc, diem.diemSo, diem.hocky phải khớp với tên bạn khai báo trong Struct của Solidity
            tr.innerHTML = `
                <td style="padding: 15px; font-weight: bold; color: #1e293b;">${maSV}</td>
                <td style="padding: 15px;">${diem.maMonHoc || "N/A"}</td>
                <td style="padding: 15px; color: #16a36a; font-weight: bold; font-size: 16px;">${diem.diemSo}</td>
                <td style="padding: 15px;">${diem.hocky || diem.hocKy || "N/A"}</td>
                <td style="padding: 15px; color: #16a36a; font-size: 13px;">
                    <span style="background: #e3f8ed; padding: 4px 8px; border-radius: 4px;">✓ Đã xác thực (Web3)</span>
                </td>
            `;
            tbody.appendChild(tr);
        });

    } catch (error) {
        console.error("Lỗi khi đọc dữ liệu Blockchain:", error);
        tbody.innerHTML = `<tr><td colspan="5" style="text-align: center; padding: 20px; color: #e11d48;">Lỗi kết nối mạng lưới hoặc sai tên hàm. Vui lòng kiểm tra Console.</td></tr>`;
    }
}