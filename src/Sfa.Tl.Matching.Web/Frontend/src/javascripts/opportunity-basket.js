//Select all checkboxes

$('.tl-selectall').click(function () {
    $('.tl-checkbox').prop('checked', this.checked);
    if ($(this).is(':checked')) {
        $(".tl-table-clickable tbody tr").addClass("checked");
    }
    else {
        $(".tl-table-clickable tbody tr").removeClass("checked");
    }
});


$(".tl-checkbox").change(function () {
    if ($('.tl-checkbox:checked').length === $('.tl-checkbox').length) {
        $('.tl-selectall').prop('checked', true);
    }

    else {
        $('.tl-selectall').prop('checked', false);
    }
});

//Select entire table row
$(".tl-table-clickable tbody tr").click(function (e) {
    if ($(e.target).is('a, a *')) {
        e.stopPropagation();
    }

    else if (e.target.type === "checkbox") {
        if ($(this).hasClass("checked")) {
            $(this).removeClass("checked");
        } else {
            $(this).addClass("checked");
        }
    }
    else {
        if ($(this).hasClass("checked")) {
            $(this).find("input.tl-checkbox").click();
            $(this).removeClass("checked");
        } else {
            $(this).find("input.tl-checkbox").click();
            $(this).addClass("checked");
        }
    }
})