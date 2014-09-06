$(document).ready(function () {
    var confirmDeleteDialog = $("#dialog-confirm").dialog({
        autoOpen: false,
        resizable: false,
        height: 140,
        modal: true,
        buttons: {
            "Continue": function () {
                finalSubmit = true;
                var form = $("#deleteAccountForm");
                $(form).submit();
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });

    var finalSubmit = false;

    $("#deleteAccountForm").submit(function () {
        if (!finalSubmit) {
            confirmDeleteDialog.dialog("open");
            return false;
        }
    });
});

function onDeleteAccountSuccess() {
    location.reload();
}