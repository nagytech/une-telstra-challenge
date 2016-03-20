lazy val root = (project in file(".")).
  settings(
    name := "cernav",
    version := "1.0",
    scalaVersion := "2.11.7"
  )

libraryDependencies += "org.twitter4j" % "twitter4j-stream" % "3.0.3"

