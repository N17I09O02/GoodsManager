window.toggleChiTiet = async function (idLanDatHang) {
    const row = document.getElementById('ct-' + idLanDatHang);
    const content = document.getElementById('ct-content-' + idLanDatHang);

    if (row.style.display === 'none') {
        row.style.display = '';
        try {
            const res = await fetch('/api/ChiTietDatHangApi/ById/' + idLanDatHang);
            if (!res.ok) {
                content.innerHTML = "<span class='text-danger'>Không thể tải chi tiết.</span>";
                return;
            }

            const data = await res.json();
            if (!data || data.length === 0) {
                content.innerHTML = "<span class='text-muted'>Không có mặt hàng nào.</span>";
                return;
            }

            // ✅ Giao diện bảng chi tiết + nút chọn tất cả
            let html = `
                <button class="btn btn-sm btn-outline-success mb-2" onclick="chonTatCaDaGiao(${idLanDatHang})">
                    Chọn tất cả là Đã giao
                </button>
                <table class="table table-sm table-hover mt-2">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Mặt hàng</th>
                            <th>Số lượng</th>
                            <th>Tổng tiền</th>
                            <th>Trạng thái</th>
                        </tr>
                    </thead>
                    <tbody>
            `;

            for (let item of data) {
                html += `
                    <tr>
                        <td>${item.idChiTietDonHang}</td>
                        <td>${item.idMatHang}</td>
                        <td>${item.soLuong}</td>
                        <td>${item.tongTien.toLocaleString()} đ</td>
                        <td>
                            <select class="form-select form-select-sm" onchange="capNhatTrangThai(${item.idChiTietDonHang}, ${idLanDatHang}, this.value)">
                                <option value="0" ${Number(item.trangThai) === 0 ? 'selected' : ''}>Chưa giao</option>
                                <option value="1" ${Number(item.trangThai) === 1 ? 'selected' : ''}>Đã giao</option>
                                <option value="2" ${Number(item.trangThai) === 2 ? 'selected' : ''}>Đã hủy</option>
                            </select>
                        </td>
                    </tr>
                `;
            }

            html += "</tbody></table>";
            content.innerHTML = html;

            // ✅ Cập nhật trạng thái đơn hàng tổng
            await capNhatTrangThaiDon(idLanDatHang, data);

        } catch (err) {
            content.innerHTML = "<span class='text-danger'>Lỗi khi tải dữ liệu.</span>";
        }
    } else {
        row.style.display = 'none';
    }
};

window.capNhatTrangThai = async function (idChiTiet, idLanDatHang, newTrangThai) {
    try {
        const response = await fetch('/api/ChiTietDatHangApi', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                idChiTietDonHang: idChiTiet,
                trangThai: parseInt(newTrangThai)
            })
        });

        if (!response.ok) {
            alert('❌ Cập nhật trạng thái thất bại');
        } else {
            console.log(`✅ Đã cập nhật trạng thái ID ${idChiTiet} thành ${newTrangThai}`);
            await capNhatTrangThaiDon(idLanDatHang);
        }
    } catch (err) {
        alert('❌ Có lỗi xảy ra khi gửi yêu cầu cập nhật');
    }
};


window.capNhatTrangThaiDon = async function (idLanDatHang, chiTietList) {
    if (!chiTietList) {
        const res = await fetch('/api/ChiTietDatHangApi/ById/' + idLanDatHang);
        if (!res.ok) return;
        chiTietList = await res.json();
    }

    let daGiao = 0, huy = 0, chuaGiao = 0;

    for (let item of chiTietList) {
        if (item.trangThai === 1) daGiao++;
        else if (item.trangThai === 2) huy++;
        else chuaGiao++;
    }

    const div = document.getElementById("trang-thai-" + idLanDatHang);
    if (!div) return;

    if (chuaGiao === 0) {
        // Tất cả đã giao, không có cái nào bị huỷ hoặc chờ
        div.innerText = "✅ Hoàn thành";
        div.className = "text-success fw-bold";
    }
    else {
        // Tất cả chưa giao
        div.innerText = "⏳ Chưa hoàn thành";
        div.className = "text-warning fw-bold";
    }

};

window.chonTatCaDaGiao = async function (idLanDatHang) {
    const res = await fetch('/api/ChiTietDatHangApi/ById/' + idLanDatHang);
    const data = await res.json();

    for (let item of data) {
        await capNhatTrangThai(item.idChiTietDonHang, idLanDatHang, 1);
    }

    await capNhatTrangThaiDon(idLanDatHang);
};
