$(document).ready(function () {
    var remainingHours;
    var startDate;
    var endDate;
    var checked;
    var hoursToLog;

    function padDate(date) {
        return date < 10 ? "0" + date : date;
    }

    $(function () {
        $("#RecurrenceDays").css("visibility", "hidden");
    })

    $(function () {
        var id = $('#hours').data('id');
        var startDate = $("#daterangeStartDate").val();
        var endDate = $('#daterangeEndDate').val();
        $.ajax({
            url: "MyActivity/GetHours?employeeID=" + id + "&startDate=" + startDate + "&endDate=" + endDate,
            success: function (data) {
                $("#budgetHours").html(data.BudgetHours);
                $("#loggedHours").html(data.LoggedHours);
                $("#remainingHours").html(data.RemainingHours);
                $("#remainingCategories").html(data.PendingCategories);
            }
        });
    })


    $('#projects2').change(function () {
        getCategoriesByProject(this);
    })

    $('#projects').change(function () {
        getCategoriesByProject(this);
    })

    function getCategoriesByProject(project) {
        var val = $(project).val();
        $(".categories").empty();
        $.getJSON('MyActivity/GetCategoriesByProject?projectId=' + val, function (result) {
            $("<option>").attr("value", "").text("").appendTo(".categories");
            $.each(result, function () {
                $("<option>").attr("value", this.CategoryId).text(this.Category).appendTo(".categories");
            });
        });
    }

    $("#btn-add-entry").on("click", function () {
        startDate = document.getElementById("startDate").value;
        endDate = document.getElementById("endDate").value;
        var id = $('#modal-add-entry').data('id');
        var hours = $("#modal-entry-hours").val();
        $.ajax({
            url: "MyActivity/GetHoursForDateRange?startDate=" + startDate + "&endDate=" + endDate + "&employeeID=" + id
        }).done(function (data) {
            remainingHours = data.RemainingHours;
            var remainingHoursToShow;
            if (data.RemainingHours - hours < 0) {
                remainingHoursToShow = 0;
            } else {
                remainingHoursToShow = data.RemainingHours - hours
            }
            $(".remainingHours").html(remainingHoursToShow);
            $("#modal-entry-hours").attr("max", data.RemainingHours);
            if (data.RemainingHours == 0) {
                $(".submitForm").attr("disabled", true);
            };
        })
        $(".date-picker").on("change", function () {
            $(".submitForm").attr("disabled", false);
            startDate = document.getElementById("startDate").value;
            endDate = document.getElementById("endDate").value;
            var id = $('#modal-add-entry').data('id');
            $.ajax({
                url: "MyActivity/GetHoursForDateRange?startDate=" + startDate + "&endDate=" + endDate + "&employeeID=" + id
            }).done(function (data) {
                remainingHours = data.RemainingHours;
                var hours = document.getElementById("modal-entry-hours").value;
                var newRemainingHours = data.RemainingHours - hours;
                var parsedStartDate = new Date(document.getElementById("startDate").value);
                var parsedEndDate = new Date(document.getElementById("endDate").value);
                var timeDiff = Math.abs(parsedStartDate.getTime() - parsedEndDate.getTime());
                var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;
                var weeks;
                if (diffDays < 7) {
                    weeks = 1;
                } else {
                    weeks = Math.ceil((parsedEndDate - parsedStartDate + 1) / (1000 * 60 * 60 * 24) / 7)
                }
                if ($('#recurrenceCheckbox').prop("checked") && diffDays > 0) {
                    var boxes = $('input[type="checkbox"]:checked');
                    checked = boxes.length;
                    if (checked == 0) {
                        checked = 1;
                    }
                    if (diffDays < (checked - 1)) {
                        totalHoursToLog = hours * weeks * diffDays;
                    } else {
                        totalHoursToLog = hours * weeks * (checked - 1);
                    }
                    newRemainingHours = data.RemainingHours - totalHoursToLog;
                }
                if (newRemainingHours < 0) {
                    $(".remainingHours").html("0");
                    $("#modal-entry-hours").attr("max", "0");
                    $(".submitForm").attr("disabled", true);
                } else {
                    $(".remainingHours").html(newRemainingHours);
                    $("#modal-entry-hours").attr("max", data.RemainingHours);
                    if (data.RemainingHours <= 0) {
                        $(".submitForm").attr("disabled", true);
                    };
                }
            })
        })
    })

    $(".checkbox-inline :checkbox, #modal-entry-hours").on('change input', function () {
        if ($('#recurrenceCheckbox').prop("checked")) {
            $("#RecurrenceDays").css("visibility", "visible");
        } else {
            $("#RecurrenceDays").css("visibility", "hidden");
            $('.checkbox-inline').prop("checked", false);

        }
        var startDate = new Date(document.getElementById("startDate").value);
        var endDate = new Date(document.getElementById("endDate").value);
        var timeDiff = Math.abs(startDate.getTime() - endDate.getTime());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24)) + 1;
        var weeks;
        if (diffDays < 7) {
            weeks = 1;
        } else {
            weeks = Math.ceil((endDate - startDate + 1) / (1000 * 60 * 60 * 24) / 7)
        }
        var totalHoursToLog;
        var hours;
        if (diffDays > 0 && $('#recurrenceCheckbox').prop("checked")) {
            var boxes = $('input[type="checkbox"]:checked');
            checked = boxes.length;
            if (checked == 0) {
                checked = 1;
            }
            hours = document.getElementById("modal-entry-hours").value;
            if (diffDays < (checked - 1)) {
                totalHoursToLog = hours * weeks * diffDays;
            } else {
                totalHoursToLog = hours * weeks * (checked - 1);
            }
        } else {
            hours = document.getElementById("modal-entry-hours").value;
            totalHoursToLog = hours;
        }
        var newRemainingHours;
        newRemainingHours = remainingHours - totalHoursToLog;
        if (newRemainingHours < 0) {
            newRemainingHours = 0;
            $(".submitForm").attr("disabled", true);
        } else {
            $(".submitForm").attr("disabled", false);
        }
        if (checked == 5 && newRemainingHours == 0) {
            newRemainingHours = hours;
            $("#modal-entry-hours").attr("max", newRemainingHours);
            $(".remainingHours").html("0");
        } else {
            $("#modal-entry-hours").attr("max", remainingHours);
            $(".remainingHours").html(newRemainingHours);
        }
        newRemainingHours = remainingHours;
    })

    $(".edit").on("click", function () {
        var entryId = $(this).attr('data-id');
        $.ajax({
            url: "/MyActivity/GetSingleProjectEntry/" + entryId,
            cache: false
        }).done(function (data) {
            $("#projects2").val(data.ProjectID).attr("selected", "selected");
            $(function () {
                var val = $('#projects2').val();
                $(".categories").empty();
                $.getJSON('MyActivity/GetCategoriesByProject?projectId=' + val, function (result) {
                    $("<option>").attr("value", "").text("").appendTo(".categories");
                    $.each(result, function () {
                        $("<option>").attr("value", this.CategoryId).text(this.Category).appendTo(".categories");
                    });
                    $(".categories").val(data.SREDCategoryID).attr("selected", "selected");
                });
            })
            var date = new Date(parseInt(data.Date.substr(6)));
            var formattedDate = padDate(date.getMonth() + 1) + "/" + padDate(date.getDate()) + "/" + date.getFullYear();
            $("#editDate").val(formattedDate);
            $("#editDescription").val(data.Description);
            $("#editHours").val(data.Hours);
            hoursToLog = data.Hours;
            $("#hiddenEditID").val(data.ProjectEntryID);
            var startDate = document.getElementById("editDate").value;
            var endDate = startDate;
            var id = $('#editEntry').data('id');
            $.ajax({
                url: "MyActivity/GetHoursForDateRange?startDate=" + startDate + "&endDate=" + endDate + "&employeeID=" + id
            }).done(function (data) {
                remainingHours = data.RemainingHours;
                $("#editHours").attr("max", data.RemainingHours);
                if (data.RemainingHours == 0) {
                    $(".submitForm").attr("disabled", true);
                };
                var maxHours = (remainingHours * 1) + (hoursToLog * 1);
                $("#editHours").attr("max", maxHours);
                $(".remainingHours").html(remainingHours);
                $(".maxHoursLeft").html(maxHours);
            })
        });
        $(".date-picker").on("change", function () {
            var startDate = document.getElementById("editDate").value;
            var endDate = startDate;
            var id = $('#editEntry').data('id');
            $.ajax({
                url: "MyActivity/GetHoursForDateRange?startDate=" + startDate + "&endDate=" + endDate + "&employeeID=" + id
            }).done(function (data) {
                remainingHours = data.RemainingHours;
                $(".remainingHours").html(data.RemainingHours);
                $("#editHours").attr("max", data.RemainingHours);
                if (data.RemainingHours == 0) {
                    $(".submitForm").attr("disabled", true);
                };
            })
        })
    })

    $(".delete").on("click", function () {
        var entryId = $(this).data('id');
        $.ajax({
            url: "/MyActivity/GetSingleProjectEntry/" + entryId,
            cache: false
        }).done(function (data) {
            $(".deleteID").val(data.ProjectEntryID);
        });
    });
});

