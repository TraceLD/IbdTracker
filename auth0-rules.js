// email rule;
function executeEmailRule(user, context, callback) {
    context.accessToken['https://ibdsymptomtracker.com/claims/email'] = user.email;
    callback(null, user, context);
}