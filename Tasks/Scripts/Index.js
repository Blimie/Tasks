$(() => {
    const userId = Number($("#current-user-id").val());

    $.connection.hub.start();
    const hub = $.connection.tasksHub;

    $("#add-task").on('click', function () {
        const title = $("#title").val();
        $("#title").val('');
        $("#title").focus();
        hub.server.addTask({ title });
    });

    $("table").on('click', '.btn-info', function () {
        hub.server.setUserForTask($(this).data("task-id"));
    });

    $("table").on('click', '.btn-success', function () {
        hub.server.setTaskCompleted($(this).data("task-id"));
    });

    hub.client.newTaskAdded = task => {
        $("#table").append(`<tr id="TaskId-${task.Id}"><td>${task.Title}</td><td><button class="btn btn-info" style="width:200px" data-task-id="${task.Id}">I'll do this task!</button></td></tr>`);
    };

    hub.client.userWasSet = task => {
        const a = $(`button[data-task-id=${task.Id}]`);

        if (task.UserId === userId) {
            a.text("I'm Done!").removeClass('btn-info').addClass('btn-success');     
        } else {
            a.text(`${task.UserName} is doing this...`).removeClass('btn-info').addClass('btn-danger').attr('disabled', true);     
        }
    };

    hub.client.taskCompleted = id => {
        $(`#TaskId-${id}`).remove();
    };
});