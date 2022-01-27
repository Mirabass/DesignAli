// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$('[data-toggle="popover"]').popover({
    placement : 'bottom',
    html : true,
    content: function(){
            var product_id = $(this).attr("id");
    return load_image(product_id);
        }
});

function load_image(product_id){
    $.ajax({
        type: 'POST',
        url: location.origin + '/Product/RetrieveProductImage',
        dataType: 'json',
        data: { Id: product_id },
        success: function (response) {
            $('#toedit' + product_id).html('<img src="' + response + '" class="img-thumbnail" style="max-width:100%;height:auto">');
        },
        error: function (response) {
        }
    });
    return '<div id="toedit'+ product_id +'">Loading...</div>';
}