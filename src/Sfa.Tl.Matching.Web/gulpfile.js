﻿/// <binding BeforeBuild='dev' />

var gulp = require('gulp');

require('./gulp/tasks/dev');
require('./gulp/tasks/default');

gulp.task('default', ['govuk-js', 'copy-js', 'copy-employer-js', 'copy-editquals-js', 'copy-missing-quals-js', 'copy-opportunity-basket-js', 'copy-assets', 'merge-css']);
gulp.task('dev', ['govuk-js', 'copy-js', 'dev-copy-employer-js', 'dev-copy-editquals-js', 'dev-copy-missing-quals-js', 'dev-copy-opportunity-basket-js', 'copy-assets', 'merge-css']);


