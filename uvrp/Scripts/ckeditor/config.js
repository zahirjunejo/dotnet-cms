/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	config.uiColor = '#C0C0C0';
	config.extraPlugins = 'imageuploader';
	config.filebrowserImageBrowseUrl = '/CKEditor/uploadPartial';
	config.filebrowserImageUploadUrl = '/CKEditor/uploadnow';
	
};
