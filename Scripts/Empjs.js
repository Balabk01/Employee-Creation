function submitEmployee() {
    const form = document.getElementById("employeeForm");
    const name = document.getElementById("Name").value.trim();
    const dob = document.getElementById("DateOfBirth").value;
    const email = document.getElementById("Email").value.trim();
    const picture = document.getElementById("Picture").files[0];
    const isEditing = form.dataset.editingId;
    const url = isEditing ? `/Home/EditEmployee` : `/Home/AddEmployee`;

    // Basic Validation Patterns
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const currentDate = new Date();
    const selectedDate = new Date(dob);

    // Date of Birth Validation
    if (dob === "" || selectedDate >= currentDate) {
        alert("Date of Birth must be less than the current date.");
        return;
    }

    // Email Validation
    if (!emailPattern.test(email)) {
        alert("Please enter a valid email address.");
        return;
    }

    // Image File Type Validation
    if (picture) {
        const allowedExtensions = ["image/jpeg", "image/jpg", "image/png"];
        if (!allowedExtensions.includes(picture.type)) {
            alert("Only image files (.jpg, .jpeg, .png) are allowed.");
            return;
        }
    }

    // Prepare Form Data and AJAX Submission
    const formData = new FormData(form);
    if (isEditing) formData.append("ID", isEditing);

    $.ajax({
        url: url,
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            alert(isEditing ? "Employee updated successfully!" : "Employee added successfully!");
            location.reload();
        },
        error: function () {
            alert("Failed to save employee.");
        }
    });
}


function deleteEmployee(id) {
    if (confirm("Are you sure you want to delete this employee?")) {
        $.ajax({
            url: '/Home/DeleteEmployee',
            type: 'POST',
            data: { id: id },
            success: function () {
                alert("Employee deleted successfully!");
                location.reload();
            },
            error: function () {
                alert("Failed to delete employee.");
            }
        });
    }
}

function editEmployee(id) {
    $.ajax({
        url: `/Home/GetEmployee?id=${id}`,
        type: "GET",
        dataType: "json",
        success: function (response) {
            if (response.success) {
                const employee = response.data;
                document.getElementById("Name").value = employee.Name;
                document.getElementById("DateOfBirth").value = employee.DateOfBirth;
                document.getElementById("Email").value = employee.Email;

                if (employee.PicturePath) {
                    document.getElementById("PicturePreview").src = `/Images/${employee.PicturePath}`;
                    document.getElementById("PicturePreview").style.display = "block";
                }

                document.getElementById("employeeForm").dataset.editingId = employee.ID;
                document.getElementById("Ademp").innerText = "Update Employee";
            } else {
                alert("Employee not found.");
            }
        },
        error: function () {
            alert("Failed to retrieve employee data.");
        }
    });
}

function resetEmployeeForm() {
    // Get the form element
    const form = document.getElementById("employeeForm");

    // Reset all input fields in the form
    form.reset();

    // Optionally, clear the image preview if applicable
    const picturePreview = document.getElementById("PicturePreview");
    picturePreview.src = ""; // Clear the preview image
    picturePreview.style.display = "none"; // Hide the image preview

    // Reset the editing ID to indicate a new entry
    form.dataset.editingId = "";
    document.getElementById("Ademp").innerText = "Add Employee"; // Reset the button text
}
