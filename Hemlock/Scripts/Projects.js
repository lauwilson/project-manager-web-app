$('.sred-category').data("hidden", true);
$('.sred-category .expand-row').click(function () {
    console.log("clicked on span");
    var $this = $(this).parent().parent();
    if ($this.data("executing")) {
        console.log("executing already");
        return;
    }
    $this.data("executing", true);

    if ($this.data("hidden")) {
        $this.find('.sred-category-employees').show();
        $this.find('.category-icon').removeClass('glyphicon-chevron-right');
        $this.find('.category-icon').addClass('glyphicon-chevron-down');
        $this.data("hidden", false);
        console.log("showing rows");
    } else {
        $this.find('.sred-category-employees').hide();
        $this.find('.category-icon').removeClass('glyphicon-chevron-down');
        $this.find('.category-icon').addClass('glyphicon-chevron-right');
        $this.data("hidden", true);
        console.log("hiding rows");
    }
    $this.removeData('executing');
    console.log("removing executing");

})

$('.input-daterange input').each(function () {
    $(this).datepicker();
})

$('#btn-add-project').click(function () {
    console.log('modal clicked');
    $('#modal-add-project').modal('show');
})

$('.btn-edit-category').click(function () {
    console.log('edit category modal clicked');
    var categoryID = $(this).data('id');
    console.log(categoryID);
    $.ajax({
        url: "/SREDCategory/GetCategoryById",
        dataType: 'json',
        data: { 'id' : categoryID },
        cache: false
    }).done(function (data) {
        console.log("done");
        console.log(data);
        $('#modal-edit-category-category-id').val(data.SREDCategoryID);
        $('#modal-edit-category-new-name').val(data.CategoryName);
        $('#modal-edit-category').modal('show');
    })
    .fail(function () {
        console.log("failed");
    })
})

//$('#date-submit').click(function () {
//    $.ajax({
//        url: "/Projects/Index",
//        data: {
//            'projectName': "hello",
//            'fromDate': $('#date-from').val(),
//            'toDate' : $('#date-to').val()
//        }
//    })
//    .success(function () {
//        console.log("success");
//    })
//    .fail(function () {
//        console.log("failed");
//    })
//})
