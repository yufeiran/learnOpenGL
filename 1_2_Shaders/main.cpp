//#include<glad/glad.h>
//
//#include<GLFW/glfw3.h>
//
//#include<iostream>
//
//void framebuffer_size_callback(GLFWwindow* window, int width, int height);
//void processInput(GLFWwindow* window);
//
//
//const char* vertexShaderSource = "#version 330 core\n"
//"layout (location = 0) in vec3 aPos;\n"
//"void main()\n"
//"{\n"
//"  gl_Position=vec4(aPos.x,aPos.y,aPos.z,1.0);\n"
//"}\0";
//
//const char* vertexShaderSource1 = "#version 330 core\n"
//"layout (location = 0) in vec3 aPos;\n"
//"out vec4 vertexColor;\n"
//"void main()\n"
//"{\n"
//"  gl_Position = vec4(aPos,1.0);\n"
//"  vertexColor = vec4(0.5, 0.0 , 0.0, 1.0);\n"
//"}\0";
//
//const char* vertexShaderSource2 = "#version 330 core\n"
//"layout (location = 0) in vec3 aPos;\n"
//"layout (location = 1) in vec3 aColor;\n"
//"out vec3 ourColor;\n"
//"void main()\n"
//"{\n"
//"  gl_Position=vec4(aPos,1.0);\n"
//"  ourColor=aColor;\n"
//"}\0";
//
//const char* fragmentShaderSource = "#version 330 core\n"
//"out vec4 FragColor;\n"
//"void main()\n"
//"{\n"
//"  FragColor=vec4(1.0f,0.5f,0.2f,1.0f);\n"
//"}\0";
//
//
//const char* fragmentShaderSource1 = "#version 330 core\n"
//"out vec4 FragColor1;\n"
//"void main()\n"
//"{\n"
//"  FragColor1=vec4(0.4f,0.19f,0.48f,1.0f);\n"
//"}\0";
//
//
//const char* fragmentShaderSource2 = "#version 330 core\n"
//"out vec4 FragColor;\n"
//"in vec4 vertexColor;\n"
//"void main()\n"
//"{\n"
//"  FragColor = vertexColor;"
//"}\0";
//
//const char* fragmentShaderSource3 = "#version 330 core\n"
//"out vec4 FragColor;\n"
//"uniform vec4 ourColor;"
//"void main()\n"
//"{\n"
//" FragColor = ourColor;\n"
//"}\0";
//
//const char* fragmentShaderSource4 = "#version 330 core\n"
//"out vec4 FragColor;\n"
//"in vec3 ourColor;\n"
//"void main()\n"
//"{\n"
//"  FragColor=vec4(ourColor,1.0);"
//"}\0";
//
//float vertices[] = {
//	// first triangle
//	  0.5f, 0.5f, 0.0f, // top right
//	  0.5f,-0.5f, 0.0f, // bottom right
//	 -0.5f,-0.5f, 0.0f, // bottom left
//	 -0.5f, 0.5f, 0.0f,  // top left
//
//	  1.0f, 0.0f, 0.0f,
//	  1.0f, 1.0f, 0.0f,
//	  0.0f, 1.0f, 0.0f,
//
//	  1.0f, 0.0f, 0.0f,
//	  0.0f, 1.0f, 0.0f,
//	  0.0f, 0.0f, 0.0f
//
//};
//
//float vertices3[] = {
//	 0.5f, -0.5f, 0.0f,    1.0f, 0.0f, 0.0f,
//	-0.5f, -0.5f, 0.0f,    0.0f, 1.0f, 0.0f,
//	 0.0f,  0.5f, 0.0f,    0.0f, 0.0f, 1.0f
//};
//
//unsigned int indices[] = { //start from 0!
//	0,1,3, //first triangle
//	1,2,3,  //second triangle
//	4,5,6,
//	7,8,9
//};
//
//float vertices1[] = {
//	  1.0f, 0.0f, 0.0f,
//	  1.0f, 1.0f, 0.0f,
//	  0.0f, 1.0f, 0.0f
//};
//
//float vertices2[] = {
//	  1.0f, 0.0f, 0.0f,
//	  0.0f, 1.0f, 0.0f,
//	  0.0f, 0.0f, 0.0f
//};
//
//unsigned int indices1[] = {
//	0,1,2
//};
//
//unsigned int indices2[] = {
//	2,1,0
//};
//
//
//int main()
//{
//	glfwInit();
//	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
//	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
//	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
//
//	GLFWwindow* window = glfwCreateWindow(800, 600, "LearnOpenGL", NULL, NULL);
//	if (window == NULL)
//	{
//		std::cout << "Failed to create GLFW window" << std::endl;
//		glfwTerminate();
//		return -1;
//	}
//	glfwMakeContextCurrent(window);
//
//	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress))
//	{
//		std::cout << "Failed to initialize GLAD" << std::endl;
//		return -1;
//	}
//
//	glViewport(0, 0, 800, 600);
//	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback);
//
//	int nrAttributes;
//	glGetIntegerv(GL_MAX_VERTEX_ATTRIBS, &nrAttributes);
//	std::cout << "Maximum nr of vertex attributes supported: " << nrAttributes << std::endl;
//
//	//--------------make shader-----------------------
//
//
//	unsigned int vertexShader;
//	vertexShader = glCreateShader(GL_VERTEX_SHADER);
//
//	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL);
//	glCompileShader(vertexShader);
//
//	int success;
//	char infoLog[512];
//	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success);
//	if (!success)
//	{
//		glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//
//	unsigned int vertexShader1;
//	vertexShader1 = glCreateShader(GL_VERTEX_SHADER);
//
//	glShaderSource(vertexShader1, 1, &vertexShaderSource1, NULL);
//	glCompileShader(vertexShader1);
//	glGetShaderiv(vertexShader1, GL_COMPILE_STATUS, &success);
//	if (!success)
//	{
//		glGetShaderInfoLog(vertexShader1, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int vertexShader2;
//	vertexShader2 = glCreateShader(GL_VERTEX_SHADER);
//
//	glShaderSource(vertexShader2, 1, &vertexShaderSource2, NULL);
//	glCompileShader(vertexShader2);
//	glGetShaderiv(vertexShader2, GL_COMPILE_STATUS, &success);
//	if (!success)
//	{
//		glGetShaderInfoLog(vertexShader2, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int fragmentShader;
//	fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
//	glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
//	glCompileShader(fragmentShader);
//
//	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
//
//	if (!success)
//	{
//		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int fragmentShader1;
//	fragmentShader1 = glCreateShader(GL_FRAGMENT_SHADER);
//	glShaderSource(fragmentShader1, 1, &fragmentShaderSource1, NULL);
//	glCompileShader(fragmentShader1);
//
//	glGetShaderiv(fragmentShader1, GL_COMPILE_STATUS, &success);
//
//	if (!success)
//	{
//		glGetShaderInfoLog(fragmentShader1, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int fragmentShader2;
//	fragmentShader2 = glCreateShader(GL_FRAGMENT_SHADER);
//	glShaderSource(fragmentShader2, 1, &fragmentShaderSource2, NULL);
//	glCompileShader(fragmentShader2);
//
//	glGetShaderiv(fragmentShader2, GL_COMPILE_STATUS, &success);
//
//	if (!success)
//	{
//		glGetShaderInfoLog(fragmentShader2, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int fragmentShader3;
//	fragmentShader3 = glCreateShader(GL_FRAGMENT_SHADER);
//	glShaderSource(fragmentShader3, 1, &fragmentShaderSource3, NULL);
//	glCompileShader(fragmentShader3);
//
//	glGetShaderiv(fragmentShader3, GL_COMPILE_STATUS, &success);
//
//	if (!success)
//	{
//		glGetShaderInfoLog(fragmentShader3, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int fragmentShader4;
//	fragmentShader4 = glCreateShader(GL_FRAGMENT_SHADER);
//	glShaderSource(fragmentShader4, 1, &fragmentShaderSource4, NULL);
//	glCompileShader(fragmentShader4);
//
//	glGetShaderiv(fragmentShader4, GL_COMPILE_STATUS, &success);
//
//	if (!success)
//	{
//		glGetShaderInfoLog(fragmentShader4, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
//	}
//
//
//	unsigned int shaderProgram;
//	shaderProgram = glCreateProgram();
//
//	glAttachShader(shaderProgram, vertexShader);
//	glAttachShader(shaderProgram, fragmentShader);
//	glLinkProgram(shaderProgram);
//
//	glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
//	if (!success) {
//		glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::LINK_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int shaderProgram1;
//	shaderProgram1 = glCreateProgram();
//
//	glAttachShader(shaderProgram1, vertexShader);
//	glAttachShader(shaderProgram1, fragmentShader1);
//	glLinkProgram(shaderProgram1);
//
//	glGetProgramiv(shaderProgram1, GL_LINK_STATUS, &success);
//	if (!success) {
//		glGetProgramInfoLog(shaderProgram1, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::LINK_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int shaderProgram2;
//	shaderProgram2 = glCreateProgram();
//
//	glAttachShader(shaderProgram2, vertexShader1);
//	glAttachShader(shaderProgram2, fragmentShader2);
//	glLinkProgram(shaderProgram2);
//
//	glGetProgramiv(shaderProgram2, GL_LINK_STATUS, &success);
//	if (!success) {
//		glGetProgramInfoLog(shaderProgram2, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::LINK_FAILED\n" << infoLog << std::endl;
//	}
//
//	unsigned int shaderProgram3;
//	shaderProgram3 = glCreateProgram();
//
//	glAttachShader(shaderProgram3, vertexShader2);
//	glAttachShader(shaderProgram3, fragmentShader4);
//	glLinkProgram(shaderProgram3); 
//
//	glGetProgramiv(shaderProgram3, GL_LINK_STATUS, &success);
//	if (!success) {
//		glGetProgramInfoLog(shaderProgram3, 512, NULL, infoLog);
//		std::cout << "ERROR::SHADER::VERTEX::LINK_FAILED\n" << infoLog << std::endl;
//	}
//
//
//	glDeleteShader(vertexShader);
//	glDeleteShader(vertexShader1);
//	glDeleteShader(vertexShader2);
//	glDeleteShader(fragmentShader);
//	glDeleteShader(fragmentShader1);
//	glDeleteShader(fragmentShader2);
//	glDeleteShader(fragmentShader3);
//	glDeleteShader(fragmentShader4);
//	//--------------------make mesh------------------
//
//	unsigned int VAO;
//	glGenVertexArrays(1, &VAO);
//	glBindVertexArray(VAO);
//
//	unsigned int VBO;
//	glGenBuffers(1, &VBO);
//	glBindBuffer(GL_ARRAY_BUFFER, VBO);
//	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);
//
//	unsigned int EBO;
//	glGenBuffers(1, &EBO);
//
//	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO);
//	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices), indices, GL_STATIC_DRAW);
//
//
//	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
//	glEnableVertexAttribArray(0);
//
//
//	unsigned int VAO1;
//	glGenVertexArrays(1, &VAO1);
//	glBindVertexArray(VAO1);
//
//	unsigned int VBO1;
//	glGenBuffers(1, &VBO1);
//	glBindBuffer(GL_ARRAY_BUFFER, VBO1);
//	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices1), vertices1, GL_STATIC_DRAW);
//
//	unsigned int EBO1;
//	glGenBuffers(1, &EBO1);
//
//	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO1);
//	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices1), indices1, GL_STATIC_DRAW);
//
//	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
//	glEnableVertexAttribArray(0);
//
//
//	unsigned int VAO2;
//	glGenVertexArrays(1, &VAO2);
//	glBindVertexArray(VAO2);
//
//	unsigned int VBO2;
//	glGenBuffers(1, &VBO2);
//	glBindBuffer(GL_ARRAY_BUFFER, VBO2);
//	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices2), vertices2, GL_STATIC_DRAW);
//
//	unsigned int EBO2;
//	glGenBuffers(1, &EBO2);
//
//	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO2);
//	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices2), indices2, GL_STATIC_DRAW);
//
//	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
//	glEnableVertexAttribArray(0);
//
//	unsigned int VAO3;
//	glGenVertexArrays(1, &VAO3);
//	glBindVertexArray(VAO3);
//
//	unsigned int VBO3;
//	glGenBuffers(1, &VBO3);
//	glBindBuffer(GL_ARRAY_BUFFER, VBO3);
//	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices3), vertices3, GL_STATIC_DRAW);
//
//	unsigned int EBO3;
//	glGenBuffers(1, &EBO3);
//
//	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, EBO3);
//	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices1), indices1, GL_STATIC_DRAW);
//
//	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)0);
//	glEnableVertexAttribArray(0);
//	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(float), (void*)(3 * sizeof(float)));
//	glEnableVertexAttribArray(1);
//
//
//	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);
//	glPolygonMode(GL_FRONT_AND_BACK, GL_FILL);
//
//	while (!glfwWindowShouldClose(window))
//	{
//		processInput(window);
//
//		glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
//		glClear(GL_COLOR_BUFFER_BIT);
//
//		glUseProgram(shaderProgram);
//
//
//		//glBindVertexArray(VAO1);
//		////glDrawArrays(GL_TRIANGLES, 0, 3);
//		//glDrawArrays(GL_TRIANGLES, 0, 3);
//		////glDrawArrays(GL_TRIANGLES, 7, 3);
//		////glDrawElements(GL_TRIANGLES, 6, GL_UNSIGNED_INT, 0);
//		//glBindVertexArray(0);
//
//		//glUseProgram(shaderProgram1);
//		//glBindVertexArray(VAO2);
//		//glDrawArrays(GL_TRIANGLES, 0, 3);
//		//glBindVertexArray(0);
//
//		//glUseProgram(shaderProgram2);
//		//glBindVertexArray(VAO);
//		//glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_INT, 0);
//		//glBindVertexArray(0);
//
//
//		//float timeValue = glfwGetTime();
//		//float greenValue = sin(timeValue) / 2.0f + 0.5f;
//		//int vertexColorLocation = glGetUniformLocation(shaderProgram3, "ourColor");
//		//glUseProgram(shaderProgram3);
//		//glUniform4f(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
//
//		//glBindVertexArray(VAO);
//		//glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_INT, 0);
//		//glBindVertexArray(0);
//
//		glUseProgram(shaderProgram3);
//
//		glBindVertexArray(VAO3);
//		glDrawElements(GL_TRIANGLES, 3, GL_UNSIGNED_INT, 0);
//		glBindVertexArray(0);
//
//		
//
//		glfwSwapBuffers(window);
//		glfwPollEvents();
//	}
//
//	glfwTerminate();
//	return 0;
//}
//
//void framebuffer_size_callback(GLFWwindow* window, int width, int height)
//{
//	glViewport(0, 0, width, height);
//}
//
//void processInput(GLFWwindow* window)
//{
//	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
//		glfwSetWindowShouldClose(window, true);
//}