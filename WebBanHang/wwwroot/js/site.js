// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    showQuantiyCart();
});
let showQuantiyCart = () => {
    $.ajax({
        url: "/customer/cart/GetQuantityOfCart",
        success: function (data) {
            //hien thi so luong san pham trong gio trong _FrontEnd.cshtml
            $(".showcart").text(data.qty);
        }
    });
}
$(document).on("click", ".addtocart", function (evt) {
    evt.preventDefault();
    let id = $(this).attr("data-productId");
    $.ajax({
        url: "/customer/cart/addtocartapi",
        data: { "productId": id },
        success: function (data) {
            // Thông báo kết quả
            Swal.fire({
                title: data.msg,
                text: "Bạn đã thêm thành công",
                icon: "success"
            });
            // Hiển thị số lượng sản phẩm trong giỏ
            $(".showcart").text(data.qty);
        }
    });
});
