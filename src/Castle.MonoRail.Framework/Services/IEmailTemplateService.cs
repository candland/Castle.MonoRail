// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.MonoRail.Framework
{
	using System.Collections;
	using System.Net.Mail;

	/// <summary>
	/// Represents the disacoupled service to use 
	/// MonoRail's view engine to process email templates.
	/// </summary>
	public interface IEmailTemplateService
	{
		/// <summary>
		/// Determines whether the specified template exists in the e-mail 
		/// template folder (<c>views/mail</c>).
		/// </summary>
		/// <param name="templateName">Name of the e-mail template.</param>
		/// <returns>
		/// 	<c>true</c> if the template exists; otherwise, <c>false</c>.
		/// </returns>
		bool HasMailTemplate(string templateName);

		/// <summary>
		/// Creates an instance of <see cref="MailMessage"/>
		/// using the specified template for the body
		/// </summary>
		/// <param name="templateName">Name of the template to load.
		/// Will look in <c>Views/mail</c> for that template file.</param>
		/// <param name="layoutName">Name of the layout.</param>
		/// <param name="parameters">Dictionary with parameters
		/// that you can use on the email template</param>
		/// <returns>An instance of <see cref="MailMessage"/></returns>
		MailMessage RenderMailMessage(string templateName, string layoutName, IDictionary parameters);

		/// <summary>
		/// Creates an instance of <see cref="MailMessage"/>
		/// using the specified template for the body
		/// </summary>
		/// <param name="templateName">Name of the template to load.
		/// Will look in <c>Views/mail</c> for that template file.</param>
		/// <param name="layoutName">Name of the layout.</param>
		/// <param name="parameters">Dictionary with parameters
		/// that you can use on the email template</param>
		/// <returns>An instance of <see cref="MailMessage"/></returns>
		MailMessage RenderMailMessage(string templateName, string layoutName, object parameters);

		/// <summary>
		/// Creates an instance of <see cref="MailMessage"/>
		/// using the specified template for the body
		/// </summary>
		/// <param name="templateName">Name of the template to load.
		/// Will look in <c>Views/mail</c> for that template file.</param>
		/// <param name="context">Context that represents the current request</param>
		/// <param name="controller">Controller instance</param>
		/// <param name="controllerContext">The controller context.</param>
		/// <param name="doNotApplyLayout">If <c>true</c>, it will skip the layout</param>
		/// <returns>An instance of <see cref="MailMessage"/></returns>
		MailMessage RenderMailMessage(string templateName, IEngineContext context,
		                          IController controller, IControllerContext controllerContext, bool doNotApplyLayout);
	}
}
