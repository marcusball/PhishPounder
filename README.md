Okay, so I received a random fan mail that just said “hey view our blog”, and that blog redirected me to [this site](http://tumblr.com.accounts.login.userid.452373.opl9.pw/tm/1/?next=http%3A%2F%2Fwww.tumblr.com%2videos%2F%3A%4A%4ID%1A) (THIS IS A PHISHING LINK, DO NOT ENTER YOUR EMAIL OR PASSWORD).

So, I just wrote a quick and really dirty application that will repeatedly send login requests to their fake form, just sending random characters as the email and password fields. Assuming they’re actually logging everything, this will just fill their database with unusable data. You can download it [here](https://github.com/marcusball/PhishPounder). Yes, this is a scary “exe” file. Use at your own risk, but I will personally say that it is safe to use.

Should anyone care, you can view the source code (and/or compile it yourself) on my github.

Here’s a screenshot:
![Screenshot](http://i.imgur.com/gU7Gu3o.png)

As long as that “Status” says “Redirect”, things are normal. If that changes, it’ll output the text, and you’ll know something weird has happened. In my opinion, if something weird happens, that’s a good sign.
