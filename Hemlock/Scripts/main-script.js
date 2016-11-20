$(document).ready(function () {
    console.log("document ready");
    var $i = 0;
    while ($i < 20) {
        $('.hide' + $i).hide();
        $i++;
    }
    $("td").click(function () {
        $('.hide' + $(this).data('class')).toggle();
    });

    $('.dropdown').on('show.bs.dropdown', function (e) {
        $(this).find('.dropdown-menu').first().stop(true, true).slideDown();
    });

    $('.dropdown').on('hide.bs.dropdown', function (e) {
        $(this).find('.dropdown-menu').first().stop(true, true).slideUp();
    });

    $('#edit-employee-start-date').datepicker({
        daysOfWeekDisabled: '0,6',
        todayHighlight: true,
        todayBtn: true,
        clearBtn: true
    })

    $('#edit-employee-end-date').datepicker({
        daysOfWeekDisabled: '0,6',
        todayHighlight: true,
        todayBtn: true,
        clearBtn: true
    })

    $('.input-daterange').datepicker({
        daysOfWeekDisabled: '0,6',
        todayHighlight: true,
        todayBtn: true,
        clearBtn: true
    });

    $('#newstaff-daterange').datepicker({
        daysOfWeekDisabled: '0,6',
        todayHighlight: true,
        todayBtn: true,
        clearBtn: true
    });

    $('#sidebar').hover(function () {
        if (window.innerWidth <= 768) {
            console.log("less than 768");
            $('.label-text').css('display', 'inline');
            $('.sidebar-logo, .dropdown').css({ 'visibility': 'visible', 'opacity': 1 });
        }
    }, function () {
        // on mouseout
        $('.label-text, .sidebar-logo, .dropdown').removeAttr('style');
    });
});
    function pick() {
        var selectedVal = "";
        var selected = $(".tableRow :checked");
        if (selected.length > 0) {
            selectedVal = selected.val();

            $("#view-activity-btn").attr("value", selectedVal);
            $('#flag').attr("value", selectedVal);
        }
    }
