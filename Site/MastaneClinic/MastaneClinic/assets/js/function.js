function submitAppointment() {
    var fullName = $('#first-name').val();
    var cellNumber = $('#txtCellNumber').val();
    var serviceId = $('#ddlservice').val();
    var dateTime = $('#txtdatetime').val();

    if (fullName !== "" && cellNumber !== "") {
        $.ajax(
            {
                url: "/VisitRequests/SubmitRequest",
                data: { fullName: fullName, cellNumber: cellNumber, serviceId: serviceId, dateTime: dateTime  },
                type: "GET"
            }).done(function (result) {
            if (result === "true") {
                $("#errorDiv").css('display', 'none');
                $("#SuccessDiv").css('display', 'block');
                localStorage.setItem("id", "");
            }
            else  {
                $("#errorDiv").html('خطایی رخ داده است. لطما مجدادا تلاش کنید.');
                $("#errorDiv").css('display', 'block');
                $("#SuccessDiv").css('display', 'none');

            }
        });
    }
    else {
        $("#errorDiv").html('فیلد های ستاره دار را تکمیل نمایید.');
        $("#errorDiv").css('display', 'block');
        $("#SuccessDiv").css('display', 'none');

    }
}