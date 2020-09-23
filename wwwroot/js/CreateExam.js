function onTitleChanged() {
    var content = $("#title-select option:selected").attr("data-content");
    $("#content-area").html(content);
}
function createExamButtonClick() {
    CreateExam(getExam);
}
function getExam() {
    var exam = {
        Post: {
            Title: $("#title-select option:selected").text(),
            Content: $("#title-select option:selected").attr("data-content"),
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
function CreateExam(exam) {
    $.ajax({
        url: "/Exams/CreateExam",
        type: "POST",
        data: {
            exam: exam
        },
        success: function (result) {

        }
    });
}