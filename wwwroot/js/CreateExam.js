function onTitleChanged() {
    var url = $("#title-select").val();
    if (url) {
        $("#content-area").html(GetContentOfPost(url));
    }
    else {
        $("#content-area").html("Please select post");
    }
}
function createExamButtonClick() {
    CreateExam();
}
function getExam() {
    var exam = {
        Post: {
            Title: $("#title-select option:selected").text(),
            Link: $("#title-select").val()
        },
        Questions: []
    }
    for (var i = 1; i < 5; i++) {
        exam.Questions.push(
            {
                Title: $("#question_" + i).val(),
                OptionA: $("#optionA_" + i).val(),
                OptionB: $("#optionB_" + i).val(),
                OptionC: $("#optionC_" + i).val(),
                OptionD: $("#optionD_" + i).val(),
                Answer: $("#answer_" + i).val()
            }
        );
    }

    return exam;
}
function CreateExam() {
    $.ajax({
        url: "/Exams/CreateExam",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(getExam()),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (result) {
            alert(result);
            if (result == "Operation successfull") {
                location.reload();
            }
        }
    });
}
function GetContentOfPost(link) {
    var content = "";
    $.ajax({
        url: "/Exams/GetContentOfPost",
        type: "POST",
        contentType: "application/json",
        async: false,
        data: JSON.stringify(link),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (result) {
            content = result;
        }
    });
    return content;
}
function deleteExam(id) {
    $.ajax({
        url: "/Exams/deleteExam",
        type: "POST",
        contentType: "application/json",
        async: false,
        data: JSON.stringify(id),
        success: function (result) {
            location.reload();
        }
    });
}
$(".box-option").click(function () {
    var questionId = $(this).attr("data-question-id");
    $(".question").eq(questionId - 1).find(".box-option").removeClass("selected");
    $(this).toggleClass("selected");
});
function seeResultsButtonClick(id) {
    $.ajax({
        url: "/Exams/getExamAnswers",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(id),
        success: function (result) {
            console.log(result);
            for (var i = 1; i < 5; i++) {
                var rightOptionSelector = "#option" + result[i - 1] + "_" + i;
                if ($(rightOptionSelector).parent().hasClass("selected")) {
                    $(rightOptionSelector).parent().addClass("right-answer");
                }
                else {
                    $(".question").eq(i - 1).find(".box-option.selected").addClass("wrong-answer");
                }
            }
        }
    });
}