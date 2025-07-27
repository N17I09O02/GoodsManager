export async function xemHinhAnhById(id, endpoint, containerId) {
    const container = document.getElementById(containerId || `img-container-${id}`);
    if (!container) return;

    container.innerHTML = "<em>Đang tải ảnh...</em>";

    try {
        const res = await fetch(`${endpoint}/${id}`);
        if (!res.ok) {
            container.innerHTML = "<span class='text-danger'>Không có ảnh.</span>";
            return;
        }

        const data = await res.json();
        if (!data || !data.url) {
            container.innerHTML = "<span class='text-muted'>Không có ảnh.</span>";
            return;
        }

        container.innerHTML = `<img src="${data.url}" style="max-width:200px;" class="img-thumbnail mt-1" />`;
    } catch (err) {
        container.innerHTML = "<span class='text-danger'>Lỗi khi tải ảnh.</span>";
    }
}

/**
 * Ẩn nút "Xem ảnh" nếu không có ảnh từ API
 * @param {number} id - ID của đơn hoặc lần đặt hàng
 * @param {string} endpoint - API endpoint kiểm tra ảnh (ví dụ: /api/ImageApi/get-url)
 * @param {string} buttonId - ID của nút cần ẩn nếu không có ảnh (ví dụ: btn-xem-anh-123)
 */
export async function anNutNeuKhongCoAnh(id, endpoint, buttonId) {
    try {
        const res = await fetch(`${endpoint}/${id}`);
        if (!res.ok) {
            const btn = document.getElementById(buttonId);
            if (btn) btn.style.display = "none";
        }
    } catch (err) {
        console.error(`❌ Lỗi khi kiểm tra ảnh với ID ${id}:`, err);
    }
}
