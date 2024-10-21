var fdata = new FormData();
var dragdrop = {
	drop: function (event) {
		event.preventDefault();
		fdata.delete("files");
		var files = event.dataTransfer.files;

		for (var i = 0; i < files.length; i++) {
			fdata.append("files", files[i]);
		}
		saveGalleryImages();

	},
	drag: function (event) {
		event.preventDefault();
	}
};

$("#avatarImage").change(function () {
	fdata.delete("files");
	var files = $('#avatarImage').prop("files");

	for (var i = 0; i < files.length; i++) {
		fdata.append("files", files[i]);
	}
	saveGalleryImages();
	
});